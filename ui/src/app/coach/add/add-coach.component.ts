import { Component, OnInit } from '@angular/core';
import {TrainingType} from '../../domain/trainingType';
import {CoachService} from '../../services/coach.service';

@Component({
  selector: 'app-coach-add',
  templateUrl: './add-coach.component.html',
  styleUrls: ['./add-coach.component.scss']
})
export class AddCoachComponent implements OnInit {
  firstName: string;
  lastName: string;
  trainingType: TrainingType;

  constructor(readonly coachService: CoachService) { }

  ngOnInit(): void {
  }

  async add(firstName: string, lastName: string, trainingType: TrainingType) {
    try {
      await this.coachService.add(firstName, lastName, trainingType);
    } catch (e) {
      console.log(e.message);
    }
  }
}
