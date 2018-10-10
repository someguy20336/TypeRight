// File Autogenerated by TypeRight.  DO NOT EDIT

import * as ServerObjects from "../ServerObjects"
import { callService } from "../CallServiceStuff"

/**
 * 
 * @param model 
 */
export function FunctionWithModel(model: ServerObjects.ASimpleModel, success?: (result: ServerObjects.ASimpleModel) => void, fail?: (result: any) => void): void {
	callService("/Home/FunctionWithModel", model, success, fail);
}

/**
 * 
 */
export function TestJson(success?: (result: ServerObjects.NetStandardClass) => void, fail?: (result: any) => void): void {
	callService("/Home/TestJson", { }, success, fail);
}

