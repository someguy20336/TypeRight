// File Autogenerated by TypeRight.  DO NOT EDIT
import { fetchWrapper } from "../CallServiceStuff";
import * as CustomGroup from "../CustomGroup";
import * as Models from "./Models";


/**
 * 
 * @param id 
 */
export function FromRoute_TestOverrideMultParamTypesMethod(id: number | boolean | string, abort?: AbortSignal): Promise<string> {
	return fetchWrapper("GET", `/api/TestWebApi/${id}?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 * @param id 
 */
export function GetRandoGroupObject(id: string, abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	return fetchWrapper("GET", `/api/TestWebApi?param1=val1&param2=val2`, id, abort);
}

/**
 * 
 * @param id 
 */
export function GetSomething(id: string, abort?: AbortSignal): Promise<string> {
	return fetchWrapper("GET", `/api/TestWebApi/things/${id}/action?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 */
export function GetStringList(abort?: AbortSignal): Promise<string[]> {
	return fetchWrapper("GET", `/api/TestWebApi?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 * @param id 
 * @param body 
 */
export function PutSomething(id: string, body: boolean, abort?: AbortSignal): Promise<string> {
	return fetchWrapper("PUT", `/api/TestWebApi/things/${id}/action?param1=val1&param2=val2`, body, abort);
}

/**
 * 
 */
export function TestActionResult(abort?: AbortSignal): Promise<string> {
	return fetchWrapper("GET", `/api/TestWebApi?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 */
export function TestClassActionResult(abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	return fetchWrapper("GET", `/api/TestWebApi?param1=val1&param2=val2`, null, abort);
}

/**
 * 
 * @param id 
 */
export function TestGetMethod(id: string, abort?: AbortSignal): Promise<string> {
	return fetchWrapper("GET", `/api/TestWebApi?param1=val1&param2=val2&id=${ id ?? "" }`, null, abort);
}

/**
 * 
 * @param id 
 */
export function TestOverrideMultParamTypesMethod(id: number | boolean | string, abort?: AbortSignal): Promise<string> {
	return fetchWrapper("GET", `/api/TestWebApi?param1=val1&param2=val2&id=${ id ?? "" }`, null, abort);
}

/**
 * 
 * @param id 
 */
export function TestOverrideSingleParamTypeMethod(id: number, abort?: AbortSignal): Promise<string> {
	return fetchWrapper("GET", `/api/TestWebApi?param1=val1&param2=val2&id=${ id ?? "" }`, null, abort);
}

/**
 * 
 * @param id 
 * @param body 
 */
export function WithFromQuery(id: string, body: Models.CustomGroupObj3, abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	return fetchWrapper("POST", `/api/TestWebApi?param1=val1&param2=val2&id=${ id ?? "" }`, body, abort);
}

