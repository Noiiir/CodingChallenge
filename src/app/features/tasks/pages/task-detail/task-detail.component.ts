import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TaskService } from '../../../../services/task.service';
import { Task } from '../../../../models/task.model';
import { switchMap, catchError, of } from 'rxjs';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule, 
    MatButtonModule, 
    MatInputModule, 
    RouterModule, 
    MatSelectModule,
    MatDatepickerModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="task-detail-container">
      <h2>{{isNewTask ? 'Create Task' : 'Edit Task'}}</h2>
      
      <div *ngIf="isLoading" class="loading-container">
        <mat-spinner></mat-spinner>
      </div>
      
      <div *ngIf="error" class="error-container">
        <p class="error-message">{{ error }}</p>
        <button mat-button color="primary" (click)="loadTask()">Try Again</button>
        <button mat-button (click)="navigateBack()">Back to List</button>
      </div>
      
      <form *ngIf="!isLoading && !error" [formGroup]="taskForm" (ngSubmit)="saveTask()">
        <!-- Form fields -->
        <mat-form-field appearance="fill" class="form-field">
          <mat-label>Title</mat-label>
          <input matInput formControlName="title" placeholder="Task title">
          <mat-error *ngIf="taskForm.get('title')?.hasError('required')">
            Title is required
          </mat-error>
        </mat-form-field>
        
        <mat-form-field appearance="fill" class="form-field">
          <mat-label>Description</mat-label>
          <textarea matInput formControlName="description" rows="4"></textarea>
        </mat-form-field>
        
        <mat-form-field appearance="fill" class="form-field">
          <mat-label>Due Date</mat-label>
          <input matInput [matDatepicker]="picker" formControlName="dueDate">
          <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
          <mat-datepicker #picker></mat-datepicker>
        </mat-form-field>
        
        <mat-form-field appearance="fill" class="form-field">
          <mat-label>Priority</mat-label>
          <mat-select formControlName="priority">
            <mat-option value="low">Low</mat-option>
            <mat-option value="medium">Medium</mat-option>
            <mat-option value="high">High</mat-option>
          </mat-select>
        </mat-form-field>
        
        <mat-form-field appearance="fill" class="form-field">
          <mat-label>Status</mat-label>
          <mat-select formControlName="status">
            <mat-option value="todo">To Do</mat-option>
            <mat-option value="in-progress">In Progress</mat-option>
            <mat-option value="completed">Completed</mat-option>
          </mat-select>
        </mat-form-field>
        
        <div class="action-buttons">
          <button mat-button type="button" (click)="navigateBack()">Cancel</button>
          <button mat-raised-button color="primary" type="submit" [disabled]="taskForm.invalid || isSubmitting">
            {{ isSubmitting ? 'Saving...' : 'Save' }}
          </button>
        </div>
      </form>
    </div>
  `,
  styles: [`
    .task-detail-container {
      padding: 20px;
      max-width: 600px;
      margin: 0 auto;
    }
    
    .form-field {
      width: 100%;
      margin-bottom: 16px;
    }
    
    .action-buttons {
      margin-top: 24px;
      display: flex;
      justify-content: flex-end;
      gap: 12px;
    }
    
    .loading-container, .error-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      min-height: 200px;
    }
    
    .error-message {
      color: #f44336;
      margin-bottom: 16px;
    }
  `]
})
export class TaskDetailComponent implements OnInit {
  taskForm: FormGroup;
  task: Task | null = null;
  isLoading = false;
  isSubmitting = false;
  error: string | null = null;
  isNewTask = false;
  
  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required]],
      description: [''],
      dueDate: [null],
      priority: ['medium'],
      status: ['todo']
    });
  }
  
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id === 'new') {
        this.isNewTask = true;
      } else {
        this.loadTask(Number(id));
      }
    });
  }
  
  private initializeForm(): void {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required]],
      description: [''],
      dueDate: [null],
      priority: ['medium'],
      status: ['todo']
    });
  }
  
  patchFormValues(task: Task): void {
    this.taskForm.patchValue({
      title: task.title,
      description: task.description,
      dueDate: task.dueDate ? new Date(task.dueDate) : null,
      priority: task.priority,
      status: task.status
    });
  }
  
  loadTask(id: number): void {
    this.isLoading = true;
    this.error = null;
    
    this.taskService.getTask(id).subscribe({
      next: (task) => {
        this.task = task;
        this.patchFormValues(task);
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load task. Please try again.';
        this.isLoading = false;
      }
    });
  }
  
  saveTask(): void {
    if (this.taskForm.invalid) return;
    
    this.isSubmitting = true;
    const taskData = this.taskForm.value;
    
    const request$ = this.isNewTask
      ? this.taskService.createTask(taskData)
      : this.taskService.updateTask(this.task?.id || 0, taskData);
      
    request$.subscribe({
      next: () => {
        this.isSubmitting = false;
        this.navigateBack();
      },
      error: (err) => {
        this.error = 'Failed to save task. Please try again.';
        this.isSubmitting = false;
      }
    });
  }
  
  navigateBack(): void {
    this.router.navigate(['/tasks']);
  }
}