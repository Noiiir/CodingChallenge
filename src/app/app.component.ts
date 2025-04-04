import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LayoutComponent],
  //removed layout component from the app component, makes it so that layout is only rendered by the routes
  template: '<router-outlet></router-outlet>'
})
export class AppComponent {
  title = 'Task Management System';
}
