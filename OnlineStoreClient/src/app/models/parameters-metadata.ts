export class ParametersMetaData{
    parametersSearchValues: { [parameter: string]: string }
    parametersNames: { [parameter: string]: string }
    parameters: any
    orderByColumns: string[]
    dependentSearchValues: { parameter: string, dependentOnParameter: string, dependentOnValues: any[], searchValues: any[] }[]
}