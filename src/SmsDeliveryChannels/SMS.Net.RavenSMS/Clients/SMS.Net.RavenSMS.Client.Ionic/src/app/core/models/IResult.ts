/**
 * and object that defines the operation result
 */
export interface IResult {
    /**
     * true if the operation succeeded
     */
    isSuccess: boolean;

    /**
     * and error message if any that describe the operation failure.
     */
    error?: string;
}
