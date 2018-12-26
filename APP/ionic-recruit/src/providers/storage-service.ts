import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {

    constructor() { }

    write(key: string, value: any) {
        if (value) {
            value = JSON.stringify(value);
        }
        localStorage.setItem(key, value);
    }

    read<T>(key: string): T {
        let value: string = localStorage.getItem(key);
        if (value && value != "undefined" && value != "null") {
            return <T>JSON.parse(value);
        }
        return null
    }

    remove(key: string) {
        localStorage.removeItem(key);
    }

    clear() {
        localStorage.clear();
        //sessionStorage.clear();
    }

    writeSession(key: string, value: any) {
        if (value) {
            value = JSON.stringify(value);
        }
        sessionStorage.setItem(key, value);
    }

    readSession<T>(key: string): T {
        let value: string = sessionStorage.getItem(key);
        if (value && value != "undefined" && value != "null") {
            return <T>JSON.parse(value);
        }
        return null
    }

    removeSession(key: string) {
        sessionStorage.removeItem(key);
    }

    clearSession() {
        sessionStorage.clear();
    }
}