import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { User } from '../User';
import { RoleService } from '../role.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { ChangeDetectorRef } from '@angular/core';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-role-management.component',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './role-management.component.html',
  styleUrl: './role-management.component.css'
})
export class RoleManagementComponent implements OnInit{
  users$ = new BehaviorSubject<User[]>([]);
  roles: string[] = ['Student', 'Department', 'Faculty', 'Course', 'Exam'];
  selectedRoles: { [userId: string]: string[] } = {};
  public assignMessages: { [userId: string]: string } = {};

  constructor(private roleService: RoleService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.roleService.getAllUsers().subscribe(users => {
      this.users$.next(users);

      users.forEach(user => {
        this.roleService.getUserRoles(user.id).subscribe(userRoles => {
          if (userRoles) {
            this.selectedRoles[user.id] = userRoles;
            this.cdr.detectChanges();
            console.log("Roles: ", this.selectedRoles)
          }
        });
      });

    });
  }

  assignRoles(userId: string, userName: string): void {
  const roles = this.selectedRoles[userId];
  if (roles && roles.length > 0) {
    this.roleService.assignRoles(userId, roles).subscribe({
      next: (res) => {
        this.assignMessages[userId] = res.message;
      },
      error: (err) => {
        this.assignMessages[userId] =  err.message;
      }
    });
  } else {
    this.assignMessages[userId] = `No roles selected for ${userName}.`;
  }
}


  toggleRole(userId: string, role: string, checked: boolean): void {
  const userRoles = this.selectedRoles[userId] || [];
  if (checked) {
    if (!userRoles.includes(role)) userRoles.push(role);
  } else {
    const index = userRoles.indexOf(role);
    if (index > -1) userRoles.splice(index, 1);
  }
  this.selectedRoles[userId] = [...userRoles];
}

  
}
