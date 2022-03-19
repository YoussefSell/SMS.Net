import { ActionReducer, MetaReducer } from '@ngrx/store';
import * as ActionTypes from './actions';
import { State } from '../root-state';

const statePersistenceReducer = (reducer: ActionReducer<State>): ActionReducer<State> => {
    return (state, action: any) => {
        if (action.type === ActionTypes.StateLoadingSucceeded.type) {
            return action.state;
        }

        return reducer(state, action);
    };
};

export const metaReducers: MetaReducer<any>[] = [statePersistenceReducer];
