import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TasksRoutingModule } from './tasks-routing.module';
import { TaskListComponent } from './pages/task-list/task-list.component';
import { TaskDetailComponent } from './pages/task-detail/TaskDetailComponent';
import { TaskFormComponent } from './pages/task-form/task-form.component';


@NgModule({       //declarartion statment
  declarations: [TaskListComponent, TaskDetailComponent,TaskFormComponent
  ],
  imports: [
    CommonModule,
    TasksRoutingModule
  ]
})
export class TasksModule { }
