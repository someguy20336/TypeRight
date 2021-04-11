﻿

export function fetchWrapper<T>(method: string, url: string, body: any, abortSignal?: AbortSignal): Promise<T> {

	let data: string = null;
	if (body) {
		data = JSON.stringify(body);
    }

	return new Promise<T>((resolve, reject) => {
		fetch(url, {
			method: method,
			headers: {
				"Content-Type": "application/json; charset=utf-8",
			},
			body: data,
			signal: abortSignal
		}).then((resp) => {
			resp.json().then(val => {
				resolve(val);
			}).catch((reason) => {
				console.log("Caught error: " + reason);
			});
		}).catch((reason) => {

			reject(reason);
		});
	});
}

/**
 * Calls a post action
 * @param url The URL to call
 * @param data the data to pass
 * @param success the success function
 * @param fail the fail function
 */
export function callPost<T>(url: string, data: any, abortSignal?: AbortSignal): Promise<T> {

	return new Promise<T>((resolve, reject) => {
		fetch(url, {
			method: "POST",
			headers: {
				"Content-Type": "application/json; charset=utf-8",
			},
			body: JSON.stringify(data),
			signal: abortSignal
		}).then((resp) => {
			resp.json().then(val => {
				resolve(val);
			}).catch((reason) => {
				console.log("Caught error: " + reason);
			});
		}).catch((reason) => {

			reject(reason);
		});
	});
}

export function callGet<T>(url: string, abortSignal?: AbortSignal): Promise<T> {

	return new Promise<T>((resolve, reject) => {
		fetch(url, {
			method: "GET",
			signal: abortSignal
		}).then((resp) => {
			resp.json().then(val => {
				resolve(val);
			}).catch((reason) => {
				console.log("Caught error: " + reason);
			});
		}).catch((reason) => {
			reject(reason);
		});
	});
}

export function callPut<T>(url: string, data: any, abortSignal?: AbortSignal): Promise<T> {

	return Promise.resolve(null);
}