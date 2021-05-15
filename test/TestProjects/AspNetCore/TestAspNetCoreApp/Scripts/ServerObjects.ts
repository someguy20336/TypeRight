// File Autogenerated by TypeRight.  DO NOT EDIT

// ===============================
// Classes
// ===============================
/** Tst model */
export interface ASimpleModel {
	/** Doc for prop 1 */
	propOne: number;
	/** Doc for prop 2 */
	propTwo: string;
	/**  */
	overrideSysText: number;
	/**  */
	overrideNewtonsoft: number;
	/**  */
	noNewtonsoftOverride: number;
}

/**  */
export interface ASimpleModel_1<T> {
	/**  */
	genericThing: T;
}

/**  */
export interface CommandResult {
	/**  */
	success: boolean;
	/**  */
	errorMessage: string;
}

/**  */
export interface CommandResult_1<T> extends CommandResult {
	/**  */
	result: T;
}

/** Test summ */
export interface ErrorViewModel {
	/**  */
	requestId: string;
	/**  */
	showRequestId: boolean;
}

/**  */
export interface GenericModel<T> {
	/**  */
	prop1: T;
}

/**  */
export interface ModelWithArray {
	/**  */
	simpleType: string;
	/**  */
	arrayType: number[];
}

/** Start some doc here */
export interface NetStandardClass {
	/**  */
	helloThere: number;
	/**  */
	enumType: NetStandardEnum;
}

/**  */
export interface TestTwoTypeParams<T, T2> {
	/**  */
	prop1: T;
	/**  */
	prop2: T2;
}

  

// ===============================
// Enums
// ===============================
/**  */
export enum NetStandardEnum {
	/**  */
	One = 0,
	/**  */
	Two = 1,
	/**  */
	Three = 2,
}

  