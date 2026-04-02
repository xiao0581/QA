Feature: Create Booking

This feature tests the booking functionality

@positive
Scenario: Successful booking
  Given no existing bookings
  When I create a booking from "2026-04-10" to "2026-04-12"
  Then the booking should be successful

@negative @date
Scenario: Booking in the past
  Given no existing bookings
  When I create a booking from "2020-01-01" to "2020-01-02"
  Then the booking should fail

@negative @overlap
Scenario: Overlapping booking
  Given a booking exists from "2026-04-10" to "2026-04-12"
  When I create a booking from "2026-04-11" to "2026-04-13"
  Then the booking should fail