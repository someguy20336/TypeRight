// File Autogenerated by TypeRight.  DO NOT EDIT
import { fetchWrapper } from "./CallServiceStuff";


/**
 * 
 * @param id 
 */
export function DeleteFirst(id: number, abort?: AbortSignal): Promise<any> {
	return fetchWrapper("DELETE", `/api/FirstGrouped/${id}?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 * @param id 
 */
export function DeleteSecond(id: number, abort?: AbortSignal): Promise<any> {
	return fetchWrapper("DELETE", `/api/SecondGrouped/${id}?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 */
export function GetFirst(abort?: AbortSignal): Promise<string[]> {
	return fetchWrapper("GET", `/api/FirstGrouped?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 * @param id 
 */
export function GetFirstById(id: number, abort?: AbortSignal): Promise<string> {
	return fetchWrapper("GET", `/api/FirstGrouped/${id}?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 */
export function GetSecond(abort?: AbortSignal): Promise<string[]> {
	return fetchWrapper("GET", `/api/SecondGrouped?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 * @param id 
 */
export function GetSecondById(id: number, abort?: AbortSignal): Promise<string> {
	return fetchWrapper("GET", `/api/SecondGrouped/${id}?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 * @param value 
 */
export function PostFirst(value: string, abort?: AbortSignal): Promise<any> {
	return fetchWrapper("POST", `/api/FirstGrouped?param1=val1&param2=val2`, value, abort);
}

/**
 * 
 * @param value 
 */
export function PostSecond(value: string, abort?: AbortSignal): Promise<any> {
	return fetchWrapper("POST", `/api/SecondGrouped?param1=val1&param2=val2`, value, abort);
}

/**
 * 
 * @param id 
 * @param value 
 */
export function PutFirst(id: number, value: string, abort?: AbortSignal): Promise<any> {
	return fetchWrapper("PUT", `/api/FirstGrouped/${id}?param1=val1&param2=val2`, value, abort);
}

/**
 * 
 * @param id 
 * @param value 
 */
export function PutSecond(id: number, value: string, abort?: AbortSignal): Promise<any> {
	return fetchWrapper("PUT", `/api/SecondGrouped/${id}?param1=val1&param2=val2`, value, abort);
}

