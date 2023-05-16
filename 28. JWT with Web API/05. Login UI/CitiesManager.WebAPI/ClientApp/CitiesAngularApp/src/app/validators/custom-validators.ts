import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";

export function CompareValidation(controlToValidate: string, controlToCompare: string): ValidatorFn {
  return (formGroupAsControl: AbstractControl): ValidationErrors | null => {
    const formGroup = formGroupAsControl as FormGroup;
    const control = formGroup.controls[controlToValidate];
    const matchingControl = formGroup.controls[controlToCompare];

    if (control.value != matchingControl.value) {
      formGroup.get(controlToCompare)?.setErrors({ compareValidator: { valid: false } });
      return { compareValidator: true }
    }
    else
      return null;
  }
}
