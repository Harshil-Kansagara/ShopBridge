import { HttpStatusCode } from '@angular/common/http';
import { Component, OnInit, Output, EventEmitter, AfterViewInit, ChangeDetectorRef, ChangeDetectionStrategy, AfterContentChecked } from '@angular/core';
import { ApplicationService, FilterAC, ProductAC } from '../utils/application.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AddItemComponent } from '../add-item/add-item.component';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.scss']
})
export class ItemListComponent implements OnInit, AfterContentChecked {

  searchTextChanged: Subject<string> = new Subject<string>();
  filterItemName: string = '';
  filterItemPrice: string = '';
  products: ProductAC[];
  totalItemsCount:number;
  recordSkip = 0;
  maxNumbersOfPagesToShow: number = 5;
  activePageNumber: number;
  perPageRecordCount: number;
  debounceTime: number;
  filtersArray: FilterAC[] = [];
  filterModelAsString: string='';
  search: string ='';
  constructor(private readonly applicationService:ApplicationService, 
              private readonly toasterService: ToastrService,
              private readonly router: Router,
              private readonly cdr: ChangeDetectorRef) { 
    this.products = [];
    this.perPageRecordCount = 10;
    this.totalItemsCount = 0;
    this.debounceTime = 1000;
    this.activePageNumber = 0;
  }

  ngOnInit(): void {
    this.perPageRecordCount = 10;
    this.searchTextChanged
      .pipe(debounceTime(this.debounceTime))
      .pipe(distinctUntilChanged())
      .subscribe(model => {
        this.search = model;
        this.activePageNumber = 0;
        this.getItems(this.activePageNumber, this.perPageRecordCount, null, this.filterModelAsString);
      });
    this.getItems();
  }

  ngAfterContentChecked(){
    this.cdr.detectChanges();
  }

  // getItemList(){
  //   this.applicationService.getItems().subscribe(
  //     (data: any) =>{
  //       this.products = data;
  //       this.totalItemsCount = this.products.length;
  //     },(error)=>{
  //       this.toasterService.error(error.response);
  //     }
  //   );
  // }

  getItems(pageNo = 0, pageRecordCount = this.perPageRecordCount, filters = null, filter = ''){
    this.applicationService.getItems(pageNo, pageRecordCount, filters, filter).subscribe(
      (data: any)=>{
        this.products = data.products;
        this.totalItemsCount = data.totalProductsCount
      }
    );
  }

  deleteItem(product: ProductAC){
    if(product.id !== undefined){
      if(confirm("Are you sure to delete "+product.name+" item")) {
        this.applicationService.deleteItem(product.id).subscribe(
          ()=>{
            this.toasterService.success("Product Removed Successfully");
            this.getItems();
          },(error)=>{
            this.toasterService.error(error.response);
          }
        );
      }
    }
  }

  addItem(){
    this.router.navigate(["item/add"]);
  }

  updateItem(product:ProductAC){
    this.router.navigate(["item/update",product.id]);
  }

  selectPerPageRecordCount(pageCount: any) {
    this.activePageNumber = 0;
    this.perPageRecordCount = pageCount;
    this.applicationService.getItems(this.activePageNumber, this.perPageRecordCount, null, this.filterModelAsString).subscribe(
      (data: any)=>{
        this.products = data.products;
        this.totalItemsCount = data.totalProductsCount
      }
    );
  }

  pageChanged(event: any) {
    this.activePageNumber = event.page;
    this.applicationService.getItems(this.activePageNumber, this.perPageRecordCount, null, this.filterModelAsString).subscribe(
      (data: any)=>{
        this.products = data.products;
        this.totalItemsCount = data.totalProductsCount
      }
    );
  }

  searchItemName(itemName: string){
    this.filterItemName = itemName;
    this.getFiltersArrayAsString("name", "=", itemName);
    this.searchTextChanged.next(this.filterModelAsString);
  }

  searchItemPrice(itemPrice: string){
    this.filterItemPrice = itemPrice;
    this.getFiltersArrayAsString("price", "=", itemPrice);
    this.searchTextChanged.next(this.filterModelAsString);
  }

  getFiltersArrayAsString(fieldName: string, operator: string, value: string) {
    if (value) {
      const filterAC = new FilterAC();
      filterAC.field = fieldName;
      filterAC.operator = operator;
      filterAC.value = value;
      this.filtersArray = this.filtersArray.filter(x => x.field !== fieldName);
      this.filtersArray.push(filterAC);
    } else {
      this.filtersArray = this.filtersArray.filter(x => x.field !== fieldName);
    }
    this.filterModelAsString = JSON.stringify(this.filtersArray);
  }
}
