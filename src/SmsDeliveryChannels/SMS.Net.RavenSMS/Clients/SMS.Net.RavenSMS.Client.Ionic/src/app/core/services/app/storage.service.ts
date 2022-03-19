import { Injectable } from '@angular/core';
import { Storage } from '@capacitor/storage';
import { from, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class StorageService {

    private STATE_KEY = 'STATE';
    private TOKEN_KEY = 'TOKEN';

    /**
     * save the value in storage for a given key.
     * @param key the key to save the value with it
     * @param value the value to be saved
     * @returns a promise that define the operation status
     */
    saveAsync<TValue>(key: string, value: TValue): Promise<void> {
        return Storage.set({ key, value: JSON.stringify(value) });
    }

    /**
     * Get the value from storage of a given key.
     * @param key the key to retrieve from the storage
     * @returns the value of the given key in the storage
     */
    async getAsync<TValue>(key: string): Promise<TValue | null> {
        const result = await Storage.get({ key });

        // check if we have a value
        if (result && result.value) {
            return JSON.parse(result.value ?? '') as TValue;
        }

        // if not return null
        return null;
    }

    /**
     * Remove the value from storage for a given key, if any.
     * @param key the key to remove
     * @returns a promise with the operation status
     */
    removeAsync(key: string): Promise<void> {
        return Storage.remove({ key });
    }

    /**
     * clear all the values from the storage
     * @returns a promise with the operation status
     */
    clearStorageAsync(): Promise<void> {
        return Storage.clear();
    }

    /**
     * save the value in storage for a given key.
     * @param key the key to save the value with it
     * @param value the value to be saved
     * @returns an observable promise that define the operation status
     */
    save$<TValue>(key: string, value: TValue): Observable<void> {
        return from(this.saveAsync(key, value));
    }

    /**
     * Get the value from storage of a given key.
     * @param key the key to retrieve from the storage
     * @returns the value of the given key in the storage
     */
    get$<TValue>(key: string): Observable<TValue | null> {
        return from(this.getAsync<TValue>(key));
    }

    /**
     * Remove the value from storage for a given key, if any.
     * @param key the key to remove
     * @returns an observable promise that define the operation status
     */
    remove$(key: string): Observable<void> {
        return from(this.removeAsync(key));
    }

    /**
     * clear all the values from the storage
     * @returns a promise with the operation status
     */
    clearStorage$(): Observable<void> {
        return from(this.clearStorageAsync());
    }

    /**
     * save the application state to the underlying storage
     * @param state the application state to be saved
     * @returns a void observable
     */
    saveState$<TState>(state: TState): Observable<void> {
        return this.save$(this.STATE_KEY, state);
    }

    /**
     * retrieve the state of the application from the underlying storage
     * @returns an observable with the state value
     */
    getState$<TState>(): Observable<TState | null> {
        return this.get$<TState>(this.STATE_KEY);
    }

    /**
     * retrieve the token from the storage.
     * @returns token value from the storage if exist.
     */
    getToken$(): Observable<string | null> {
        return this.get$<string>(this.TOKEN_KEY);
    }

    /**
     * save the authentication token to the storage
     * @param token the token to be saved
     * @returns a void observable
     */
    saveToken$(token: string): Observable<void> {
        return this.save$(this.TOKEN_KEY, token);
    }

    /**
     * remove the application state from storage
     */
    removeState$(): Observable<void> {
        return from(this.remove$(this.STATE_KEY));
    }

    /**
     * remove the token from storage
     */
    removeToken$(): Observable<void> {
        return from(this.remove$(this.TOKEN_KEY));
    }
}
