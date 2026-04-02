Feature: Create Booking

Tests the booking functionality
 
  @positive @ECT
  Scenario: T1 Valid booking in the future
    Given no existing bookings
    When I create a booking from "2026-04-10" to "2026-04-12"
    Then the booking should be successful

  @negative @date
  Scenario: T2 Booking in the past
    Given no existing bookings
    When I create a booking from "2020-01-01" to "2020-01-02"
    Then the booking should fail

  @negative @overlap
  Scenario: T3 Overlapping booking
    Given a booking exists from "2026-04-10" to "2026-04-12"
    When I create a booking from "2026-04-11" to "2026-04-13"
    Then the booking should fail

  @negative @overlap
  Scenario: T4 Fully inside existing booking
    Given a booking exists from "2026-04-10" to "2026-04-15"
    When I create a booking from "2026-04-11" to "2026-04-12"
    Then the booking should fail



  @positive @boundary  @BVT
  Scenario: T5 Adjacent booking (boundary)
    Given a booking exists from "2026-04-10" to "2026-04-12"
    When I create a booking from "2026-04-12" to "2026-04-14"
    Then the booking should be successful

  @negative @boundary
  Scenario: T6 Start date is today
    Given no existing bookings
    When I create a booking from "2026-04-02" to "2026-04-03"
    Then the booking should fail

  @negative
  Scenario: T7 End before start
    Given no existing bookings
    When I create a booking from "2026-04-12" to "2026-04-10"
    Then the booking should fail