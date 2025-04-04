import { Component, OnInit , ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule,MatTable } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { TaskService } from '../../../../services/task.service';
import { Task } from '../../../../models/task.model';
import { MatInput, MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule,MatSort } from '@angular/material/sort';
import { MatChipsModule } from '@angular/material/chips'
import { MatBadgeModule } from '@angular/material/badge'
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged} from 'rxjs/operators';
import { MatTableDataSource } from '@angular/material/table';
import { MatProgressSpinnerModule} from '@angular/material/progress-spinner';


@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, 
             MatTableModule, 
             MatButtonModule, 
             MatIconModule, 
             RouterModule, 
             MatInputModule, 
             MatFormFieldModule, 
             MatSelectModule, 
             MatPaginatorModule, 
             MatSortModule, 
             MatChipsModule,
             MatBadgeModule, 
             ReactiveFormsModule,
            MatProgressSpinnerModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
 
})
export class TaskListComponent implements OnInit,AfterViewInit {
  dataSource = new MatTableDataSource<Task>([]);
  displayedColumns: string[] = ['title', 'dueDate', 'priority', 'status', 'actions'];
  // for overdue dattes
  today = new Date();

  // Filters & options
  titleFilter = new FormControl('');
  priorityFilter = new FormControl('');
  statusFilter = new FormControl('');
  priorityOptions = ['Low','Medium','High'];
  statusOptions = ['Pending','InProgress','Completed'];

  //error handling / loading
  isLoading = false;
  error: string | null = null;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  



  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    this.setupFilterListeners();
    this.loadTasks();
  }

//error handlied and loading state
  loadTasks(): void {
   this.isLoading = true;
   this.error = null;

   this.taskService.getTasks().subscribe({
    next: tasks => {
      this.dataSource.data = tasks;
      this.isLoading = false;
    },
    error:err =>{
      this.error = 'Failed to load the task. Try again.';
      this.isLoading = false; 
    },

   });
  }
    //delet task added error handling
  deleteTask(id: number): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(id).subscribe({
      next: () => {
              this.loadTasks();
      },
      error: err => {
        this.error = 'Failed to delete task. Please try again.';
           }
      });
    }
  }


ngAfterViewInit() {
  //paginator and sort setup 
  this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;

    //custom date sort
    this.dataSource.sortingDataAccessor = (item: Task, property: string) => {
      switch(property) {
        case 'dueDate': 
          return item.dueDate ? new Date(item.dueDate).getTime() : 0; // Convert Date to timestamp number
        default: 
          // Make sure this always returns string or number
          const value = item[property as keyof Task];
          return value !== undefined && value !== null ? String(value) : '';
      }
    };
}  

//method for filter listeners
  setupFilterListeners(): void{
      this.titleFilter.valueChanges.pipe(
        debounceTime(400),
        distinctUntilChanged()
      ).subscribe(value => {
        this.applyFilters();
      });
    
      this.priorityFilter.valueChanges.subscribe(() =>this.applyFilters());
      this.statusFilter.valueChanges.subscribe(() => this.applyFilters() );
  }
    //method for applying filters
  applyFilters(): void{
      this.dataSource.filterPredicate = (data: Task, filter: string) => {
        const searchStr = filter.toLowerCase();
        const titileMatch = this.titleFilter.value ? data.title.toLowerCase().
        includes(this.titleFilter.value.toLowerCase()) : true;
        const priorityMatch = this.priorityFilter.value ? data.priority === this.priorityFilter.value : true;
        const statusMatch = this.statusFilter.value ? data.status === this.statusFilter.value : true;

        return titileMatch && priorityMatch && statusMatch;
      };

        //get timestamp filter, triggers the filter predicate
      this.dataSource.filter = new Date().getTime().toString();
  }


  //clearing filters
  clearFilters(): void {
    this.titleFilter.setValue('');
    this.priorityFilter.setValue('');
    this.statusFilter.setValue('');
    this.applyFilters();
  }

  getPriorityClass(priority: string): string{

    switch(priority) {
      case 'High': return 'priority-high';
      case 'Medium': return 'priority-medium';
      case 'Low': return 'priority-low';
      default: return  '';
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Completed': return 'status-completed';
      case 'InProgress':  return 'status-in-progress';
      case 'Pending' : return 'status-pending';
      default:return '';
        
    }
  }


}
