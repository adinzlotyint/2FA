import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordBoxComponent } from './passwordBox.component';

describe('passwordBoxComponent', () => {
  let component: PasswordBoxComponent;
  let fixture: ComponentFixture<PasswordBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PasswordBoxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PasswordBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
