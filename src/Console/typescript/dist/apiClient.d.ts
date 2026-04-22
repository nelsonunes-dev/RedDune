import type { SimulationInput, SimulationOutput } from "./types.js";
export interface ApiClientOptions {
    baseUrl: string;
    timeout?: number;
}
export declare class ApiClient {
    private baseUrl;
    private timeout;
    constructor(options: ApiClientOptions);
    simulate(input: SimulationInput): Promise<SimulationOutput>;
}
export declare function createApiClient(baseUrl: string): ApiClient;
//# sourceMappingURL=apiClient.d.ts.map