import { ApplicationFeatures } from 'src/app/core/constants';
import { CommonModule } from '@angular/common';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { NgModule } from '@angular/core';
import { MainEffects } from './effects';
import { MainReducer } from './reducer';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    StoreModule.forFeature(ApplicationFeatures.messages, MainReducer),
    EffectsModule.forFeature([MainEffects])
  ]
})
export class MessagesStoreModule { }
