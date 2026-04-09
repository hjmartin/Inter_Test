import { AbstractControl, FormGroup, ValidationErrors } from '@angular/forms';

export function extraerErrores(obj: any): string[] {
  const errors = obj.error.errors;

  let mensajesDeError: string[] = [];

  for (let llave in errors) {
    let campo = llave;
    const mensajesConCampos = errors[llave].map((mensaje: string) => `${campo}: ${mensaje}`)
    mensajesDeError = mensajesDeError.concat(mensajesConCampos);
  }
  return mensajesDeError;
}

export function extraerErroresIdentity(obj: any): string[] {

  let mensajesDeError: string[] = [];

  for (let i = 0; i < obj.error.length; i++) {
    const elemento = obj.error[i];
    mensajesDeError.push(elemento.description);
  }

  return mensajesDeError;

}

export function getErrorMensajeNumero(formulario: FormGroup, campo: string): string {
  const control: AbstractControl | null = formulario.get(campo);

  if (!control || !control.errors || !control.touched) return '';

  if (control.hasError('required')) {
    return 'Este campo es obligatorio.';
  }

  if (control.hasError('pattern')) {
    return 'Solo se permiten números enteros positivos.';
  }

  if (control.hasError('min')) {
    return `El valor mínimo permitido es ${control.getError('min').min}.`;
  }

  if (control.hasError('max')) {
    return `El valor máximo permitido es ${control.getError('max').max}.`;
  }

  return 'Valor no válido.';
}

export function getErrorMensajeTexto(form: FormGroup, controlName: string): string {
  const control = form.get(controlName);
  if (!control || !control.errors || !control.touched) return '';

  if (control.hasError('required')) return 'Este campo es obligatorio.';
  if (control.hasError('minlength')) {
    const { requiredLength, actualLength } = control.getError('minlength');
    return `Debe tener al menos ${requiredLength} caracteres. Actualmente tiene ${actualLength}.`;
  }
  if (control.hasError('maxlength')) {
    const { requiredLength } = control.getError('maxlength');
    return `No debe superar los ${requiredLength} caracteres.`;
  }

  return 'Valor no válido.';
}

export function getErrorMensajeEmail(form: FormGroup, controlName: string): string {
  const control = form.get(controlName);
  if (!control || !control.errors || !control.touched) return '';

  if (control.hasError('required')) return 'El correo es obligatorio.';
  if (control.hasError('email')) return 'Debe ser un correo electrónico válido.';

  return 'Correo no válido.';
}

export function getErrorMensajeSelect(form: FormGroup, controlName: string): string {
  const control = form.get(controlName);
  if (!control || !control.errors || !control.touched) return '';

  if (control.hasError('required')) return 'Debe seleccionar una opción.';

  return 'Selección no válida.';
}

export function noFechaFuturaValidator(control: AbstractControl): ValidationErrors | null {
  const valor = control.value;
  if (!valor) return null;

  const fechaSeleccionada = new Date(valor);
  const hoy = new Date();
  hoy.setHours(0, 0, 0, 0); // Ignora la hora

  return fechaSeleccionada > hoy ? { fechaFutura: true } : null;
}
