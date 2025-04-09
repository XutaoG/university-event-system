import { AfterViewInit, Component, inject, OnInit } from '@angular/core';
import { ModalService } from '../../../services/modal/modal.service';
import { control, latLng, map, Map, tileLayer } from 'leaflet';
import { GeoSearchControl, OpenStreetMapProvider } from 'leaflet-geosearch';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { AddEventForm } from '../../../types/event-types';
import { PublicEventsService } from '../../../services/events/public-events/public-events.service';
import { Router } from '@angular/router';
import { PrivateEventsService } from '../../../services/events/private-events/private-events.service';
import { RsoEventsService } from '../../../services/events/rso-events/rso-events.service';

@Component({
	selector: 'app-add-event-modal-view',
	imports: [ReactiveFormsModule],
	templateUrl: './add-event-modal-view.component.html',
	styleUrls: [
		'./add-event-modal-view.component.scss',
		'../modal.component.scss',
	],
})
export class AddEventModalViewComponent implements AfterViewInit {
	private router = inject(Router);

	private modalService = inject(ModalService);
	private publicEventService = inject(PublicEventsService);
	private privateEventService = inject(PrivateEventsService);
	private rsoEventService = inject(RsoEventsService);
	private formBuilder = inject(FormBuilder);

	isFetching = false;
	addEventForm = this.formBuilder.group({
		name: new FormControl<string>('', { nonNullable: true }),
		category: new FormControl<string>('', { nonNullable: true }),
		description: new FormControl<string>('', { nonNullable: true }),
		date: new FormControl<Date>(new Date(), { nonNullable: true }),
		startTime: new FormControl<string>('', { nonNullable: true }),
		endTime: new FormControl<string>('', { nonNullable: true }),
		contactPhone: new FormControl<string>('', { nonNullable: true }),
		contactEmail: new FormControl<string>('', { nonNullable: true }),
		locationName: new FormControl<string>('', { nonNullable: true }),
	});

	private map!: Map;

	private searchControl!: any;

	address = '';
	latitude = 0;
	longitude = 0;

	ngAfterViewInit() {
		this.initializeMap();
	}

	initializeMap() {
		// Initialize map
		this.map = map('map', {
			center: latLng(28.6024, -81.2001),
			zoom: 12,
		});

		// Steal my key if you want IDC
		const key = 'P7irY23fbv4TpNZWg5e4';

		// Add map
		tileLayer(
			`https://api.maptiler.com/maps/streets-v2/{z}/{x}/{y}.png?key=${key}`,
			{
				//style URL
				tileSize: 512,
				zoomOffset: -1,
				minZoom: 1,
				attribution:
					'\u003ca href="https://www.maptiler.com/copyright/" target="_blank"\u003e\u0026copy; MapTiler\u003c/a\u003e \u003ca href="https://www.openstreetmap.org/copyright" target="_blank"\u003e\u0026copy; OpenStreetMap contributors\u003c/a\u003e',
				crossOrigin: true,
			}
		).addTo(this.map);

		// Add scale to map
		control.scale().addTo(this.map);

		// Add search bar
		const provider = new OpenStreetMapProvider();

		this.searchControl = GeoSearchControl({
			provider: provider,
			style: 'bar',
			updateMap: true,
		});

		this.map.addControl(this.searchControl);

		this.map.on('geosearch/showlocation', (e) => {
			// @ts-ignore
			this.address = e.location.label;
			// @ts-ignore
			this.latitude = e.location.x;
			// @ts-ignore
			this.longitude = e.location.y;
		});
	}

	onCancel() {
		this.modalService.setAddingEvent(false);
	}

	onAddEvent() {
		if (this.addEventForm.valid) {
			this.isFetching = true;

			// Assemble form data
			const formData: AddEventForm = {
				name: this.addEventForm.value.name!,
				eventCategory: this.addEventForm.value.category!,
				description: this.addEventForm.value.description!,
				eventDate: this.addEventForm.value.date!,
				eventTime: this.addEventForm.value.startTime!,
				eventTimeEnd: this.addEventForm.value.endTime!,
				location: {
					name: this.addEventForm.value.locationName!,
					latitude: this.latitude,
					longitude: this.longitude,
					address: this.address,
				},
				contactPhone: this.addEventForm.value.contactPhone!,
				contactEmail: this.addEventForm.value.contactEmail!,
			};

			if (this.router.url.startsWith('/event/public')) {
				// Post public event
				this.publicEventService
					.addPublicEvent(formData)
					.subscribe(() => {
						this.isFetching = false;
						this.onCancel();
						this.publicEventService
							.getAllPublicEvents()
							.subscribe();
					});
			} else if (this.router.url.startsWith('/event/private')) {
				// Post private event
				this.privateEventService
					.addPrivateEvent(formData)
					.subscribe(() => {
						this.isFetching = false;
						this.onCancel();
						this.privateEventService
							.getAllPrivateEvents()
							.subscribe();
					});
			} else if (this.router.url.startsWith('/rso')) {
				if (this.modalService.currentRsoId != null) {
					this.rsoEventService
						.addRsoEvent(formData, this.modalService.currentRsoId)
						.subscribe(() => {
							this.isFetching = false;
							this.onCancel();
							this.rsoEventService.getAllRsoEvents().subscribe();
						});
				}
			}
		}
	}
}
