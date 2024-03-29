// File Autogenerated by TypeRight.  DO NOT EDIT
import { fetchWrapper } from "../CallServiceStuff.js";

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
export function notRootedPath(id: number, abort?: AbortSignal): Promise<string> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/base/not-rooted/test/${id}${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 */
export function rootedPath(id: number, abort?: AbortSignal): Promise<string> {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/rooted/test/${id}${getQueryString(urlParams)}`, null, abort);
}

