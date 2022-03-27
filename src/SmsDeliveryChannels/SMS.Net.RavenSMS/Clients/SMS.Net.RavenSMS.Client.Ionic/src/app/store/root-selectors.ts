import { MessagesStoreSelectors } from './messages-store';
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { State } from './root-state';

const featureStateSelector = createFeatureSelector<State>('app');

/**
 * select global errors form all stores
 */
export const ErrorsSelector = createSelector(
    MessagesStoreSelectors.ErrorSelector,
    (error) => {
        return [
            error
        ];
    }
);

export const ServerConnectionSelector = createSelector(
    featureStateSelector,
    (state: State) => state.serverConnection
);

export const NetworkConnectionSelector = createSelector(
    featureStateSelector,
    (state: State) => state.networkConnection
);