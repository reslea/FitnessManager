import { Component, OnInit } from '@angular/core';
import {TrainingType} from '../../domain/trainingType';
import {CoachService} from '../../services/coach.service';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-coach-add',
  templateUrl: './add-coach.component.html',
  styleUrls: ['./add-coach.component.scss']
})
export class AddCoachComponent implements OnInit {
  firstName: string;
  lastName: string;
  trainingType: TrainingType;

  constructor(readonly coachService: CoachService,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
    const trainingTypeKey = 'trainingType';
    this.route.params.subscribe(params => this.trainingType = params[trainingTypeKey]);
  }

  async add(firstName: string, lastName: string, trainingType: TrainingType) {
    try {
      await this.coachService.add(firstName, lastName, trainingType);
    } catch (e) {
      console.log(e.message);
    }
  }
}
