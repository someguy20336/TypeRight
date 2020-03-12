// File Autogenerated by TypeRight.  DO NOT EDIT
import { callPost, callGet, callPut } from "../CallServiceStuff";
/**
 *
 * @param id
 */
export function GetRandoGroupObject(id, abort) {
    return callPost(`/api/TestWebApi`, id, abort);
}
/**
 *
 * @param id
 */
export function GetSomething(id, abort) {
    return callGet(`/api/TestWebApi/things/${id}/action`, abort);
}
/**
 *
 */
export function GetStringList(abort) {
    return callPost(`/api/TestWebApi`, {}, abort);
}
/**
 *
 * @param id
 * @param body
 */
export function PutSomething(id, body, abort) {
    return callPut(`/api/TestWebApi/things/${id}/action`, body, abort);
}
/**
 *
 */
export function TestActionResult(abort) {
    return callPost(`/api/TestWebApi`, {}, abort);
}
/**
 *
 */
export function TestClassActionResult(abort) {
    return callPost(`/api/TestWebApi`, {}, abort);
}
/**
 *
 * @param id
 */
export function TestGetMethod(id, abort) {
    return callGet(`/api/TestWebApi?id=${id}`, abort);
}
/**
 *
 * @param id
 * @param body
 */
export function WithFromQuery(id, body, abort) {
    return callPost(`/api/TestWebApi?id=${id}`, body, abort);
}
//# sourceMappingURL=CustomNamedActions.js.map