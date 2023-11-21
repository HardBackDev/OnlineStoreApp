import { Injectable } from '@angular/core';
import { environment } from '../enviroments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Category } from '../models/category';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ParametersMetaData } from '../models/parameters-metadata';
import { ManipulatingMetdata } from '../models/manipulating-metadata';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  url = `${environment.apiurl}/categories`

  constructor(private httpContext: HttpClient, private fb: FormBuilder) { }

  public getCategories() : Observable<Category[]> {
    return this.httpContext.get<Category[]>(this.url)
  }

  public getCategoryParametersMetadata(category: string) : Observable<ParametersMetaData> {
    return this.httpContext.get<ParametersMetaData>(`${this.url}/${category}/parameters`)
  }

  public getCategoryManipulatingMetadata(category: string) : Observable<ManipulatingMetdata> {
    return this.httpContext.get<ManipulatingMetdata>(`${this.url}/${category}/manipulatingMetadata`)
  }

  public createFormGroup(formGroup: FormGroup, object: any, values: {[parameter: string]: string} = null) {
    for (const key in object) {
      if (object.hasOwnProperty(key)) {
        formGroup.addControl(key, this.fb.control('', Validators.required))
        if(values){
          if(values[key] != null){
            formGroup.get(key).setValue(values[key][0])
          }
        }
      }
    }
  }

  public parseParametersToControls(controlls: { parameter: string; parameterName: string; valueType: string;}[], metadata: ParametersMetaData){
    for(var parameter in metadata.parameters)
    {
      var type: string = metadata.parameters[parameter]
  
      controlls.push({
        parameter: parameter,
        parameterName: metadata.parametersNames[parameter] ?? parameter,
        valueType: type
      })  
    }
  }

  public parseManipulatingMetadataToControls(controlls: { property: string; type: string; }[], metadata: ManipulatingMetdata){
    for(var controll in metadata.manipulatingObject){
      var type: string = metadata.manipulatingObject[controll]
  
      controlls.push({
        property: controll,
        type: type
      })  
    }
  }

}
