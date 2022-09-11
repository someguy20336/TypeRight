// File Autogenerated by TypeRight.  DO NOT EDIT
import { fetchWrapper } from "./CallServiceStuff.js";

function tryAppendKeyValueToUrl(urlParams: URLSearchParams, key: string, value: any): void {
    if (value !== null && typeof value !== "undefined") {
        if (Array.isArray(value)) {
            for (let aryVal of value) {
                urlParams.append(key, aryVal.toString());
            }
        } else {
            urlParams.append(key, value);
        }
    }
}

function getQueryString(urlParams: URLSearchParams): string {
    let queryString = urlParams.toString();
    if (queryString !== "") {
        queryString = "?" + queryString;
    }
    return queryString;
}

/**
 * 
 * @param id 
 */
export function deleteFirst(id: number, abort?: AbortSignal): Promise<any> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("DELETE", `/api/FirstGrouped/${id}${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 */
export function deleteSecond(id: number, abort?: AbortSignal): Promise<any> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("DELETE", `/api/SecondGrouped/${id}${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 */
export function getFirst(abort?: AbortSignal): Promise<string[]> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/FirstGrouped${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 */
export function getFirstById(id: number, abort?: AbortSignal): Promise<string> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/FirstGrouped/${id}${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 */
export function getSecond(abort?: AbortSignal): Promise<string[]> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/SecondGrouped${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 */
export function getSecondById(id: number, abort?: AbortSignal): Promise<string> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/SecondGrouped/${id}${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param value 
 */
export function postFirst(value: string, abort?: AbortSignal): Promise<any> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("POST", `/api/FirstGrouped${getQueryString(urlParams)}`, value, abort);
}

/**
 * 
 * @param value 
 */
export function postSecond(value: string, abort?: AbortSignal): Promise<any> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("POST", `/api/SecondGrouped${getQueryString(urlParams)}`, value, abort);
}

/**
 * 
 * @param id 
 * @param value 
 */
export function putFirst(id: number, value: string, abort?: AbortSignal): Promise<any> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("PUT", `/api/FirstGrouped/${id}${getQueryString(urlParams)}`, value, abort);
}

/**
 * 
 * @param id 
 * @param value 
 */
export function putSecond(id: number, value: string, abort?: AbortSignal): Promise<any> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("PUT", `/api/SecondGrouped/${id}${getQueryString(urlParams)}`, value, abort);
}
