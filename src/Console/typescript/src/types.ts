export interface RobotInput {
  x: number;
  y: number;
  orientation: string;
  commands: string;
}

export interface SimulationInput {
  gridWidth: number;
  gridHeight: number;
  robots: RobotInput[];
}

export interface RobotOutput {
  x: number;
  y: number;
  orientation: string;
  isLost: boolean;
}

export interface SimulationOutput {
  robots: RobotOutput[];
}

export interface ParsedInput {
  gridWidth: number;
  gridHeight: number;
  robots: RobotInput[];
}

export interface ParseError {
  line: number;
  message: string;
}