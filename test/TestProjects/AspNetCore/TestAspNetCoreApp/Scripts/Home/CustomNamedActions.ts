// File Autogenerated by TypeRight.  DO NOT EDIT
import { callPost, callGet, callPut } from "../CallServiceStuff";
import * as CustomGroup from "../CustomGroup";
import * as Models from "./Models";


/**
 * 
 * @param id 
 */
export function GetRandoGroupObject(id: string, abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	return callPost(`/api/TestWebApi?param1=val1&param2=val2`, id, abort);
}

/**
 * 
 * @param id 
 */
export function GetSomething(id: string, abort?: AbortSignal): Promise<string> {
	return callGet(`/api/TestWebApi/things/${id}/action?param1=val1&param2=val2`, abort);
}

/**
 * 
 */
export function GetStringList(abort?: AbortSignal): Promise<string[]> {
	return callPost(`/api/TestWebApi?param1=val1&param2=val2`, {}, abort);
}

/**
 * 
 * @param id 
 * @param body 
 */
export function PutSomething(id: string, body: boolean, abort?: AbortSignal): Promise<string> {
	return callPut(`/api/TestWebApi/things/${id}/action?param1=val1&param2=val2`, body, abort);
}

/**
 * 
 */
export function TestActionResult(abort?: AbortSignal): Promise<string> {
	return callPost(`/api/TestWebApi?param1=val1&param2=val2`, {}, abort);
}

/**
 * 
 */
export function TestClassActionResult(abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	return callPost(`/api/TestWebApi?param1=val1&param2=val2`, {}, abort);
}

/**
 * 
 * @param id 
 */
export function TestGetMethod(id: string, abort?: AbortSignal): Promise<string> {
	return callGet(`/api/TestWebApi?param1=val1&param2=val2&id=${ id ?? "" }`, abort);
}

/**
 * 
 * @param id 
 */
export function TestOverrideMultParamTypesMethod(id: number | boolean | string, abort?: AbortSignal): Promise<string> {
	return callGet(`/api/TestWebApi?param1=val1&param2=val2&id=${ id ?? "" }`, abort);
}

/**
 * 
 * @param id 
 */
export function TestOverrideSingleParamTypeMethod(id: number, abort?: AbortSignal): Promise<string> {
	return callGet(`/api/TestWebApi?param1=val1&param2=val2&id=${ id ?? "" }`, abort);
}

/**
 * 
 * @param id 
 * @param body 
 */
export function WithFromQuery(id: string, body: Models.CustomGroupObj3, abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	return callPost(`/api/TestWebApi?param1=val1&param2=val2&id=${ id ?? "" }`, body, abort);
}

