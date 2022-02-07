import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

// stores imports
import { MessagesStoreModule } from './messages-store';
import { UIStoreModule } from './ui-store';

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
