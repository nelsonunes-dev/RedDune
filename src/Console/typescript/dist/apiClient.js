"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ApiClient = void 0;
exports.createApiClient = createApiClient;
class ApiClient {
    baseUrl;
    timeout;
    constructor(options) {
        this.baseUrl = options.baseUrl.replace(/\/$/, "");
        this.timeout = options.timeout ?? 30000;
    }
    async simulate(input) {
        const url = `${this.baseUrl}/api/simulation`;
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), this.timeout);
        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(input),
                signal: controller.signal,
            });
            clearTimeout(timeoutId);
            if (!response.ok) {
                const errorText = await response.text().catch(() => "Unknown error");
                throw new Error(`API error ${response.status}: ${errorText}`);
            }
            const result = (await response.json());
            return result;
        }
        catch (error) {
            clearTimeout(timeoutId);
            if (error instanceof Error && error.name === "AbortError") {
                throw new Error(`Request timeout after ${this.timeout}ms`);
            }
            throw error;
        }
    }
}
exports.ApiClient = ApiClient;
function createApiClient(baseUrl) {
    return new ApiClient({ baseUrl });
}
//# sourceMappingURL=apiClient.js.map