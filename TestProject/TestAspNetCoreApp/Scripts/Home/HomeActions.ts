// File Autogenerated by TypeRight.  DO NOT EDIT

import * as ServerObjects from "../ServerObjects"
import { callService } from "../CallServiceStuff"

/**
 * 
 */
export function TestJson(success?: (result: ServerObjects.NetStandardClass) => void, fail?: (result: any) => void): void {
	callService("/Home/TestJson", { 
	}, success, fail);
}


