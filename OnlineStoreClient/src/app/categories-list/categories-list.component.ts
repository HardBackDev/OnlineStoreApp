import { Component } from '@angular/core';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category';

@Component({
  selector: 'app-categories-list',
  templateUrl: './categories-list.component.html',
  styleUrls: ['./categories-list.component.css']
})
export class CategoriesListComponent {
  categories: Category[] = []
  isLoading: boolean = true;

  constructor(private service: CategoryService){}

  ngOnInit(){
    this.isLoading = true
    this.service.getCategories()
    .subscribe((res: Category[]) => {
      this.categories = res
      this.isLoading = false
    })
  }
}
