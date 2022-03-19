import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ApplicationFeatures } from 'src/app/core/constants';
import { State } from './state';

const featureStateSelector = createFeatureSelector<State>(ApplicationFeatures.messages);

export const StateSelector = createSelector(
  featureStateSelector,
  (state: State) => state
);

export const IsLoadingSelector = createSelector(
  featureStateSelector,
  (state: State) => state.isLoading
);

export const ErrorSelector = createSelector(
  featureStateSelector,
  (state: State) => state.error
);

/**
 * select the messages list
 */
export const MessagesSelector = createSelector(
  featureStateSelector,
  (state: State) => state.messages
);

/**
 * select the currently selected message
 */
export const SelectedMessageSelector = createSelector(
  featureStateSelector,
  (state: State) => state.selectedMessage
);
