// File Autogenerated by TypeRight.  DO NOT EDIT
import { fetchWrapper } from "../CallServiceStuff";
import * as CustomGroup from "../CustomGroup";
import * as ServerObjects from "../ServerObjects";
import * as Models from "./Models";

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

function tryAppendObjectValuesToUrl(urlParams: URLSearchParams, obj: any): void {
    for (let [key, val] of Object.entries(obj)) {
        tryAppendKeyValueToUrl(urlParams, key, val);
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
 * @param fromQueryModel 
 */
export function complexFromQuery(id: string, fromQueryModel: Partial<ServerObjects.ASimpleModel>, abort?: AbortSignal): Promise<{ id: string, PropOne: number, PropTwo: string }> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "id", id);
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	tryAppendObjectValuesToUrl(urlParams, fromQueryModel);
	return fetchWrapper("POST", `/api/TestWebApi/complex${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param fromQueryModel 
 */
export function complexWithListFromQuery(fromQueryModel: Partial<ServerObjects.ModelWithArray>, abort?: AbortSignal): Promise<{ array: number[], simple: string }> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	tryAppendObjectValuesToUrl(urlParams, fromQueryModel);
	return fetchWrapper("POST", `/api/TestWebApi/complex-list${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 */
export function fromRoute_TestOverrideMultParamTypesMethod(id: number | boolean | string, abort?: AbortSignal): Promise<string> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/TestWebApi/${id}${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 */
export function getRandoGroupObject(id: string, abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/TestWebApi${getQueryString(urlParams)}`, id, abort);
}

/**
 * 
 * @param id 
 */
export function getSomething(id: string, abort?: AbortSignal): Promise<string> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/TestWebApi/things/${id}/action${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 */
export function getStringList(abort?: AbortSignal): Promise<string[]> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/TestWebApi${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 * @param body 
 */
export function putSomething(id: string, body: boolean, abort?: AbortSignal): Promise<string> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("PUT", `/api/TestWebApi/things/${id}/action${getQueryString(urlParams)}`, body, abort);
}

/**
 * 
 */
export function testActionResult(abort?: AbortSignal): Promise<string> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/TestWebApi${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 */
export function testClassActionResult(abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/TestWebApi${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 */
export function testGetMethod(id: string, abort?: AbortSignal): Promise<string> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "id", id);
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/TestWebApi${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 */
export function testOverrideMultParamTypesMethod(id: number | boolean | string, abort?: AbortSignal): Promise<string> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "id", id);
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/TestWebApi${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 */
export function testOverrideSingleParamTypeMethod(id: number, abort?: AbortSignal): Promise<string> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "id", id);
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("GET", `/api/TestWebApi${getQueryString(urlParams)}`, null, abort);
}

/**
 * 
 * @param id 
 * @param body 
 */
export function withFromQuery(id: string, body: Models.CustomGroupObj3, abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, "id", id);
	tryAppendKeyValueToUrl(urlParams, "param1", "val1");
	tryAppendKeyValueToUrl(urlParams, "param2", "val2");
	return fetchWrapper("POST", `/api/TestWebApi${getQueryString(urlParams)}`, body, abort);
}

