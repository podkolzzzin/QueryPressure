import { Buffer } from 'buffer';
import * as CryptoJS from 'crypto-js';

import { LocalStorageService } from './LocalStorageService';
import { UrlService } from './UrlService';

function encryptData(key: string, data: string): string {
    const cipherText = CryptoJS.AES.encrypt(data, key).toString();
    const base64 = Buffer.from(cipherText).toString('base64');
    const urlSafe = encodeURIComponent(base64);
    return urlSafe;
}

function decryptData(key: string, data: string): string {
    const urlSafe = decodeURIComponent(data);
    const base64 = Buffer.from(urlSafe, 'base64').toString('binary');
    const bytes = CryptoJS.AES.decrypt(base64, key);
    const originalData = bytes.toString(CryptoJS.enc.Utf8);
    return originalData;
}

function generateKey(length = 32): string {
    const array = new Uint8Array(length);
    window.crypto.getRandomValues(array);
    return Array.from(array).map(b => b.toString(16).padStart(2, '0')).join('');
}


export const ConnectionStringService = {
    load() : string {
        const connectionStringEncrpyted = UrlService.get('connection-string');
        if (connectionStringEncrpyted) {
            return decryptData(this.key(), connectionStringEncrpyted);
        }
        return '';
    },

    store(connectionString: string) : void {
        const encrypted = encryptData(this.key(), connectionString);
        document.location.hash = encrypted;
    },

    key() : string {
        const k = LocalStorageService.get<string>('connection-key');
        if (!k) {
            LocalStorageService.set('connection-key', generateKey());
            return this.key();
        }
        return k;
    },
};