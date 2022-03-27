/**
 * this interface defines the authentication module state
 */
export interface State {
    /**
     * true if dark mode is enabled false if not
     */
    darkMode: boolean;

    /**
     * the current selected language
     */
    language: string;
}
