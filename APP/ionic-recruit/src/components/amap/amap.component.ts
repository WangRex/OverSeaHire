import {Component, OnDestroy, OnInit, AfterViewInit, Input, ElementRef} from '@angular/core';

declare var AMap;
declare var AMapUI;

@Component({
  selector: 'ngx-amap',
  templateUrl: './amap.component.html',
})
export class AMapComponent implements OnInit, OnDestroy, AfterViewInit {
  AMapId: string;
  ruler: any;
  mouseTool: any;
  toolBar: any;
  scaleBar: any;
  overViewBar: any;
  layersPanel: any;
  infoWindow: any;
  showToolAction: boolean = false;
  @Input() mapOpts: any;
  @Input() showScale: boolean = false;
  @Input() showToolBar: boolean = false;
  @Input() showOverview: boolean = false;
  @Input() showToolBarRuler: boolean = false;
  @Input() showLayerSwitcher: boolean = false;
  @Input() showRangingTool: boolean = false;
  @Input() showAreaTool: boolean = false;
  @Input() showToggleButton: boolean = false;
  @Input() mapSuffix: string = '';
  @Input() PathSimplifier: Array<any> = [];
  @Input() markers: any = [];
  @Input() showInfo: boolean = true;

  constructor(private el: ElementRef) {
    this.AMapId = 'container' + Math.floor(Math.random() * 1000000) + this.mapSuffix;
  }

  ngOnInit() {
    // Author: John - 2018/4/2
    // 如果没有传递地图初始化参数，自动初始化参数
    if (!this.mapOpts) {
      this.mapOpts = {
        resizeEnable: true,
        zoom: 11,
        // layers: [new AMap.TileLayer.Satellite()],
      }
    }
  }

  // ngAfterViewInit Begin
  ngAfterViewInit() {
    const currentPoint = this;
    // 比例尺
    currentPoint.scaleBar = new AMap.Scale({
      visible: this.showScale,
    });
    // 缩放工具栏
    currentPoint.toolBar = new AMap.ToolBar({
      visible: this.showToolBar,
    });
    // 右下角鹰眼
    currentPoint.overViewBar = new AMap.OverView({
      visible: this.showOverview,
    });
    // 初始化地图
    const map = new AMap.Map(this.AMapId, this.mapOpts);
    map.addControl(currentPoint.scaleBar);
    map.addControl(currentPoint.toolBar);
    map.addControl(currentPoint.overViewBar);

    // 控制显示工具栏条标尺
    if (this.showToolBarRuler === false) {
      currentPoint.toolBar.hideRuler();
    }

    // 是否显示图层切换功能
    if (this.showLayerSwitcher === true) {
      AMapUI.loadUI(['control/BasicControl'], function (BasicControl) {
        // 图层切换控件
        // currentPoint.layersPanel = new BasicControl.LayerSwitcher({
        //   position: 'tr',
        // });
        // map.addControl(currentPoint.layersPanel);

        const layerCtrl2 = new BasicControl.LayerSwitcher({
          // 自定义基础图层
          baseLayers: [{
            id: 'tile',
            name: '标准图',
            layer: new AMap.TileLayer(),
          }, {
            enable: true,
            id: 'satellite',
            name: '卫星图',
            layer: new AMap.TileLayer.Satellite(),
          }],
          // 自定义覆盖图层
          overlayLayers: [{
            id: 'traffic',
            name: '路况图',
            layer: new AMap.TileLayer.Traffic(),
          }, {
            id: 'roadNet',
            name: '路网图',
            layer: new AMap.TileLayer.RoadNet(),
          }],
        });
        map.layers = layerCtrl2.getEnabledLayers();
        map.addControl(layerCtrl2);
      });
    }

    // Author: John - 2018/4/3
    // 是否显示测量距离功能
    if (this.showRangingTool === true) {
      map.plugin(['AMap.RangingTool'], function () {
        currentPoint.ruler = new AMap.RangingTool(map);
        AMap.event.addListener(currentPoint.ruler, 'end', function (e) {
          currentPoint.ruler.turnOff();
        });
      });
    }

    // Author: John - 2018/4/3
    // 是否显示测量面积功能
    if (this.showAreaTool === true) {
      map.plugin(['AMap.MouseTool'], function () {
        currentPoint.mouseTool = new AMap.MouseTool(map);
        // 鼠标工具插件添加draw事件监听
        AMap.event.addListener(currentPoint.mouseTool, 'draw', function callback(e) {
          // var eObject = e.obj;//obj属性就是鼠标事件完成所绘制的覆盖物对象。
        });
      });
    }

    // 信息窗体
    currentPoint.infoWindow = new AMap.InfoWindow({
      offset: new AMap.Pixel(0, -30),
      content: '',
      // isCustom: true,
    });
    // 默认添加点标记
    if (currentPoint.markers.length > 0) {
      currentPoint.markers.forEach(function (item) {
        let marker;
        if (item.CustomIcon === true) {
          marker = new AMap.Marker({
            map: map,
            icon: new AMap.Icon({
              image: 'assets/icon/location.png',
              imageSize: new AMap.Size(item.imageWidth, item.imageHeight),
            }),
            position: [item.Longitude, item.Latitude],
          });
        } else {
          marker = new AMap.Marker({
            map: map,
            icon: new AMap.Icon({
              image: 'assets/icon/location.png',
              // imageSize: new AMap.Size(item.imageWidth, item.imageHeight),
            }),
            position: [item.Longitude, item.Latitude],
          });
        }

        if (currentPoint.showInfo) {
          let info = [];
          info.push("<p class='input-item'>" + item.OfficeName + "</p>");
          info.push("<p class='input-item'>电话 : " + item.ContactPhone + "</p>");
          info.push("<p class='input-item'>地址 : " + item.OfficeAddress  + "</p>");
          currentPoint.infoWindow.setContent(info.join(""));
          currentPoint.infoWindow.open(map, marker.getPosition());
        }
        // marker.setExtData(item);
        //
        // // 鼠标点击点标记事件
        // marker.on('dblclick', function (e) {
        //   // currentPoint.markerClick(item, map, e);
        // });
        //
        // // 鼠标滑过点标记事件
        // marker.on('mouseover', function (e) {
        //   currentPoint.markerMouseOver(item, map, e);
        // });
        //
        // // 鼠标离开点标记事件
        // marker.on('mouseout', function (e) {
        //   currentPoint.infoWindow.close();
        // });

        currentPoint.markers.push(marker);
      });
      // 自适应显示多个点标记
      map.setFitView();
    }

    // 轨迹
    if (currentPoint.PathSimplifier.length > 0) {
      const traceInfo = currentPoint.PathSimplifier;

      const greenPoints = [];
      const redPoints = [];
      const path = [];

      traceInfo.forEach(item => {
        path.push([item.longitude, item.latitude]);
        const point = {
          position: [
            item.longitude,
            item.latitude,
          ],
        };

        if (item.qualified === '0') {
          greenPoints.push(point);
        } else {
          redPoints.push(point);
        }
      });

      // 轨迹点颜色设置
      if (greenPoints.length > 0) {
        currentPoint.initPointSimplifier(map, greenPoints, 'green');
      }

      if (redPoints.length > 0) {
        currentPoint.initPointSimplifier(map, redPoints, 'red');
      }

      // 轨迹设置
      const tracePath = [{
        'info': traceInfo,
        'path': path,
      }];
      currentPoint.setNavigationTrace(map, tracePath);
    }
  }

  // ngAfterViewInit End

  // Author: John - 2018/4/3
  // 开启测量工具
  startMapRangingTool() {
    this.ruler.turnOn();
  }

  // Author: John - 2018/4/3
  // 开启面积测量工具
  startMeasureArea() {
    this.mouseTool.measureArea({
      strokeStyle: 'dashed',
      fillColor: '#ffffff',
      fillOpacity: 0,
    });
  }

  // Author: John - 2018/4/3
  // 清除面积
  clearMeasureArea() {
    this.mouseTool.close(true);
  }

  // Author: John - 2018/4/3
  // 显示或者隐藏工具菜单
  toggleToolActions() {
    this.showToolAction = !this.showToolAction;
    if (this.showToolAction === false) {
      this.toolBar.hide();
      this.overViewBar.hide();
      this.el.nativeElement.querySelector('.amap-ui-control-layer').style.display = 'none';
    } else {
      this.toolBar.show();
      this.overViewBar.show();
      this.el.nativeElement.querySelector('.amap-ui-control-layer').style.display = 'block';
    }
  }

  // 鼠标滑过marker事件
  markerMouseOver(item, map, e) {
    const content = this.generateMarkerContent(item.type);
    this.infoWindow.setContent(content);
    this.infoWindow.open(map, e.target.getPosition());
    // this.el.nativeElement.querySelector('.amap-info-close').style.display = 'none';
  }

  // 关闭信息窗体
  closeInfoWindow() {
    this.infoWindow.close();
  }

  ngOnDestroy() {
    this.markers = [];
  }

  // 设置路径点标注
  initPointSimplifier(map, data, pointColor) {
    AMapUI.load(['ui/misc/PointSimplifier', 'lib/$'], function (PointSimplifier, $) {
      const pointSimplifierIns = new PointSimplifier({
        map: map, // 所属的地图实例
        getPosition: function (dataItem) {
          // 返回数据项的经纬度，AMap.LngLat实例或者经纬度数组
          return dataItem.position;
        },
        renderOptions: {
          // 点的样式
          pointStyle: {
            fillStyle: pointColor, // 填充颜色
          },
          topNAreaStyle: {
            'autoGlobalAlphaAlpha': true,
            'content': 'none',
            'fillStyle': '#e25c5d',
            'lineWidth': 1,
            'strokeStyle': null,
          },
        },
      });
      pointSimplifierIns.setData(data);
    });
    // map.setFitView();
  }

  // 轨迹路径回放
  setNavigationTrace(map, tracePath) {
    const that = this;
    AMapUI.load(['ui/misc/PathSimplifier', 'lib/$'], function (PathSimplifier, $) {
      const pathSimplifierIns = new PathSimplifier({
        zIndex: 100,
        // autoSetFitView:false,
        map: map, // 所属的地图实例
        // 渲染路径
        getPath: function (pathData, pathIndex) {
          return pathData.path;
        },
        renderOptions: {
          renderAllPointsIfNumberBelow: 100,
          pathLineStyle: {
            dirArrowStyle: true,
          },
          getPathStyle: function (pathItem, zoom) {
            const color = '#dc3912', lineWidth = 3;
            return {
              pathLineStyle: {
                strokeStyle: color,
                lineWidth: lineWidth,
              },
              pathLineSelectedStyle: {
                lineWidth: lineWidth,
              },
              pathNavigatorStyle: {
                fillStyle: color,
              },
            };
          },
        },
      });

      // window.pathSimplifierIns = pathSimplifierIns;

      // 定义轨迹路径的经纬度
      const flyRoutes = [];
      tracePath.push.apply(tracePath, flyRoutes);
      pathSimplifierIns.setData(tracePath);

      function onload() {
        pathSimplifierIns.renderLater();
      }

      function onerror(e) {
        // alert('图片加载失败！');
      }

      /* pathSimplifierIns.on('pointClick', function (e, data) {
        //设置单点详情并跳转
        let pointInfo = data.pathData.info[data.pointIndex];
        nav.push(FarmPointDetailPage, { PointInfo: pointInfo });
      }); */

      // 创建一个巡航器
      const navigator = pathSimplifierIns.createPathNavigator(0, {
        loop: true,
        speed: 300,
        pathNavigatorStyle: {
          width: 24,
          height: 24,
          // 使用图片
          content: PathSimplifier.Render.Canvas.getImageContent('assets/images/map/icon_nav_car.png', onload, onerror),
          strokeStyle: '#dc3912',
          lineWidth: 3,
          fillStyle: null,
          // 经过路径的样式
          pathLinePassedStyle: {
            lineWidth: 3,
            strokeStyle: '#3366cc',
            dirArrowStyle: {
              stepSpace: 15,
              strokeStyle: '#ff9900',
            },
          },
        },
      });
      // 开始执行
      navigator.start();
      // AMap.event.addDomListener(document.getElementById('pause'), 'click', function () {
      //   that.showResumeBtn = true;
      //   navigator.pause();
      //   that.ref.markForCheck();
      //   that.ref.detectChanges();
      // });
      // AMap.event.addDomListener(document.getElementById('resume'), 'click', function () {
      //   that.showResumeBtn = false;
      //   navigator.resume();
      //   that.ref.markForCheck();
      //   that.ref.detectChanges();
      // })
    });
  }

  generateMarkerContent(item) {
    let content = '';
    return content;
  }
}
