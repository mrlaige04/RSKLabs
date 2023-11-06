import { Component } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ApiResult} from "./ApiResult";
import {AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Frontend';
  inputType: 'file' | 'form' = 'file'

  form: FormGroup;

  result: ApiResult | undefined = undefined;
  constructor(private http: HttpClient, private fb: FormBuilder) {
    this.form = this.fb.group({
      rows: this.fb.array([])
    });
  }

  onFileSelected(event: any) {
    let file = event.target.files[0];
    let fd = new FormData();
    fd.append('file', file);
    this.http.post<ApiResult>('https://localhost:7089/api/file', fd)
      .subscribe((result)=>
      {
        this.result = result;
      })
  }

  get rows() : FormArray<FormGroup> {
    return this.form.get('rows') as FormArray;
  }

  getCells(group: FormGroup): FormArray<FormControl> {
    return group.get('cells') as FormArray;
  }

  createRow(): FormGroup {
    return this.fb.group({
      cells: this.fb.array([])
    });
  }

  createCell(): FormControl {
    return new FormControl('', [Validators.required])
  }

  addRow(): void {
    const rows = this.form.get('rows') as FormArray;
    rows.push(this.createRow());
  }

  addCell(row: FormGroup): void {
    const cells = row.get('cells') as FormArray;
    cells.push(this.createCell());
  }
}

