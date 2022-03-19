import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ApplicationFeatures } from 'src/app/core/constants';
import { State } from './state';

const featureStateSelector = createFeatureSelector<State>(ApplicationFeatures.settings);

export const StateSelector = createSelector(
  featureStateSelector,
  (state: State) => state
);

export const ServerInfoSelector = createSelector(
  featureStateSelector,
  (state: State) => state.serverInfo
);

export const AppIdentificationSelector = createSelector(
  featureStateSelector,
  (state: State) => state.appIdentification
);

export const IsConfiguredSelector = createSelector(
  featureStateSelector,
  (state: State) =>
    state.appIdentification.clientId !== null
    && state.appIdentification.clientId !== undefined
);
