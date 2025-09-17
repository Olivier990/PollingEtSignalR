import { Component, OnInit } from '@angular/core';
import { UselessTask } from '../models/UselessTask';
import { HttpClient } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-polling',
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    FormsModule,
    CommonModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
  ],
  templateUrl: './polling.component.html',
  styleUrls: ['./polling.component.css'],
})
export class PollingComponent implements OnInit {
  apiUrl = 'https://localhost:7289/api/';
  title = 'labo.signalr.ng';
  tasks: UselessTask[] = [];
  taskname: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.updateTasks();
  }

  async complete(id: number) {
    // TODO On invoke la méthode pour compléter une tâche sur le serveur (Contrôleur d'API)
    await lastValueFrom(this.http.get<UselessTask[]>(this.apiUrl + "UselessTasks/Complete/" + id));
    console.log("======= Je polle ======");
  }

  async addtask() {
    // TODO On invoke la méthode pour ajouter une tâche sur le serveur (Contrôleur d'API)

    let x = await lastValueFrom(this.http.post<any>(this.apiUrl + "UselessTasks/Add?taskText=" + this.taskname, {}))
    console.log(x);
  }

  async updateTasks() {
    // TODO: Faire une première implémentation simple avec un appel au serveur pour obtenir la liste des tâches
    this.tasks = await lastValueFrom(this.http.get<UselessTask[]>(this.apiUrl + "UselessTasks/GetAll"));
    console.log("Polling:", this.tasks);
    // TODO: UNE FOIS QUE VOUS AVEZ TESTER AVEC DEUX CLIENTS: Utiliser le polling pour mettre la liste de tasks à jour chaque seconde
    //setTimeout(() => this.updateTasks(), 1000);
  }
}
