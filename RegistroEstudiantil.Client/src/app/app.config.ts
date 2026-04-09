// src/app/app.config.ts
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './interceptores/auth.interceptor';
import { errorInterceptor } from './interceptores/error.interceptor';
import { loadingInterceptor } from './interceptores/loading.interceptor';
import { retryInterceptor } from './interceptores/retry.interceptor';
import { timingInterceptor } from './interceptores/timing.interceptor';
import { provideAnimations } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(
      withFetch(),
      withInterceptors([
        authInterceptor,
        loadingInterceptor,
        retryInterceptor,
        timingInterceptor,
        errorInterceptor
      ])
    ),
    provideAnimations()
  ]
};
