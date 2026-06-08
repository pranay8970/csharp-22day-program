<script setup>
import { ref, onMounted, computed } from 'vue'

// ✅ Data
const departments = ref([])
const isLoading = ref(false)

// ✅ Fetch data
const loadDepartmentData = async () => {
  try {
    isLoading.value = true

    const response = await fetch(
      "https://localhost:7130/api/analytics/department-load"
    )

    departments.value = await response.json()
  } catch (error) {
    console.error("Error fetching department load:", error)
  } finally {
    isLoading.value = false
  }
}

// ✅ Load on start
onMounted(() => {
  loadDepartmentData()
})

// ✅ Find busiest department
const maxTotal = computed(() => {
  if (departments.value.length === 0) return 0
  return Math.max(...departments.value.map(d => d.total))
})

// ✅ Grand Total (sum of all departments)
const grandTotal = computed(() => {
  return departments.value.reduce((sum, d) => sum + d.total, 0)
})
</script>

<template>
  <h1>Department Analytics</h1>


  <table border="1" cellpadding="8" cellspacing="0">
    <thead style="background-color: #ddd;">
      <tr>
        <th>Department</th>
        <th>Inpatient</th>
        <th>Outpatient</th>
        <th>ED</th>
        <th>Total</th>
      </tr>
    </thead>

    <tbody>
      <tr
        v-for="d in departments"
        :key="d.departmentName"
        :style="{
          backgroundColor: d.total === maxTotal ? '#ff4d4d' : ''
        }"
      >
        <td><b>{{ d.departmentName }}</b></td>
        <td>{{ d.inpatient }}</td>
        <td>{{ d.outpatient }}</td>
        <td>{{ d.ed }}</td>
        <td><b>{{ d.total }}</b></td>
      </tr>
    </tbody>

    <!-- ✅ GRAND TOTAL ROW -->
    <tfoot>
      <tr style="font-weight: bold; background-color: #f2f2f2;">
        <td colspan="4" style="text-align: right;">
          Grand Total:
        </td>
        <td style="text-align: center;">
          {{ grandTotal }}
        </td>
      </tr>
    </tfoot>
  </table>
</template>