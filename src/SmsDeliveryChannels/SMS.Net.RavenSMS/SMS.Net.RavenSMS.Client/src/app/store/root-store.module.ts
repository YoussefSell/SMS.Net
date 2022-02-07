import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { UIStoreModule } from '.';

// stores imports
import { MessagesStoreModule } from './messages-store';

const modules = [
    UIStoreModule,
    MessagesStoreModule,
];
@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        ...modules,
    ],
    exports: [
        ...modules
    ]
})
export class RootStoreModule { }
