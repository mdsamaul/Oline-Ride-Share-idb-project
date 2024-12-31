// import { Routes } from '@angular/router';

// export const routes: Routes = [];


// import { NgModule } from '@angular/core';
// import { RouterModule, Routes } from '@angular/router';
// import { LoginComponent } from './login/login.component';  // LoginComponent ইম্পোর্ট করুন

// const routes: Routes = [
//   { path: 'login', component: LoginComponent },  // /login পাথে LoginComponent রেন্ডার হবে
//   // আরও রুট আপনি এখানে যোগ করতে পারেন
// ];

// @NgModule({
//   imports: [RouterModule.forRoot(routes)],  // রুট কনফিগারেশন লোড
//   exports: [RouterModule]  // RouterModule এক্সপোর্ট করা হচ্ছে
// })
// export class AppRoutingModule { }

import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';  // Import the LoginComponent

// Define your routes here
export const routes: Routes = [
  { path: 'login', component: LoginComponent }, // Example route for LoginComponent
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Default route
  // Add other routes as necessary
];
