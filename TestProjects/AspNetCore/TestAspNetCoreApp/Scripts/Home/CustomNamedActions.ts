// File Autogenerated by TypeRight.  DO NOT EDIT
import { callPost, callGet, callPut } from "../CallServiceStuff";
import * as CustomGroup from "../CustomGroup";
import * as Models from "./Models";


/**
 * 
 * @param id 
 */
export function GetRandoGroupObject(id: string, abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	return callPost(`/api/TestWebApi`, id, abort);
}

/**
 * 
 * @param id 
 */
export function GetSomething(id: string, abort?: AbortSignal): Promise<string> {
	return callGet(`/api/TestWebApi/things/${id}/action`, abort);
}

/**
 * 
 */
export function GetStringList(abort?: AbortSignal): Promise<string[]> {
	return callPost(`/api/TestWebApi`, {}, abort);
}

/**
 * 
 * @param id 
 * @param body 
 */
export function PutSomething(id: string, body: boolean, abort?: AbortSignal): Promise<string> {
	return callPut(`/api/TestWebApi/things/${id}/action`, body, abort);
}

/**
 * 
 */
export function TestActionResult(abort?: AbortSignal): Promise<string> {
	return callPost(`/api/TestWebApi`, {}, abort);
}

/**
 * 
 */
export function TestClassActionResult(abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	return callPost(`/api/TestWebApi`, {}, abort);
}

/**
 * 
 * @param id 
 */
export function TestGetMethod(id: string, abort?: AbortSignal): Promise<string> {
	return callGet(`/api/TestWebApi?id=${ id ?? "" }`, abort);
}

/**
 * 
 * @param id 
 * @param body 
 */
export function WithFromQuery(id: string, body: Models.CustomGroupObj3, abort?: AbortSignal): Promise<CustomGroup.CustomGroupObject1> {
	return callPost(`/api/TestWebApi?id=${ id ?? "" }`, body, abort);
}

