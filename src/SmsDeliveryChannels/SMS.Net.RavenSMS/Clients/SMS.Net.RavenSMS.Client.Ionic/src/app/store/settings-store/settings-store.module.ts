import { ApplicationFeatures } from 'src/app/core/constants';
import { CommonModule } from '@angular/common';
import { EffectsModule } from '@ngrx/effects';
import { MainReducer } from './reducer';
import { StoreModule } from '@ngrx/store';
import { NgModule } from '@angular/core';
import { MainEffects } from './effects';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    StoreModule.forFeature(ApplicationFeatures.settings, MainReducer),
    EffectsModule.forFeature([MainEffects])
  ]
})
export class SettingsStoreModule { }
