import { ApplicationFeatures } from '../core/constants';
import { UIStoreState } from './ui-store';

/**
 * the root state of the application
 */
export interface State {
    /**
     * the ui state
     */
    [ApplicationFeatures.ui]: UIStoreState.State;
}
