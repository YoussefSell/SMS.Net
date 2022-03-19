import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { IMessages } from '../../models';

@Injectable({ providedIn: 'root' })
export class MessagesService {
    constructor() { }

    /**
     * load list of all messages
     */
    loadMessages$(): Observable<IMessages[]> {
        return of([]);
    }
}
