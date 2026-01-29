# SmartLib Management System
> **"Chuyển đổi số toàn diện nghiệp vụ Thư viện: Nâng tầm trải nghiệm tri thức."**

---

## Executive Summary (Tổng quan điều hành)

### The Problem (Thách thức thị trường)
Các hệ thống quản lý thư viện truyền thống đang đối mặt với "nút thắt cổ chai" về vận hành: quy trình mượn trả thủ công tốn kém nhân lực, khó khăn trong việc kiểm soát thất thoát tài sản (sách quá hạn, hư hỏng), và thiếu các báo cáo thời gian thực để ra quyết định nhập sách hiệu quả. Người đọc ngày nay đòi hỏi trải nghiệm số hóa, nhanh chóng và minh bạch.

### The Solution (Giải pháp của chúng tôi)
**SmartLib Management System** không chỉ là một phần mềm quản lý kho sách. Đây là một nền tảng **Enterprise Resource Planning (ERP) thu nhỏ dành riêng cho thư viện**. Chúng tôi kết hợp kiến trúc **.NET** mạnh mẽ với quy trình nghiệp vụ chặt chẽ (Business Logic Strictness) để tự động hóa 90% các tác vụ thủ công, từ việc nhập liệu, tính toán phạt, đến báo cáo thống kê.

---

## Key Value Propositions (Giá trị cốt lõi)

| Giá trị | Mô tả chi tiết |
| :--- | :--- |
| **Operational Excellence** | Giảm 80% thời gian xử lý mượn/trả nhờ quy trình tự động hóa. Hệ thống tự động tính toán ngày quá hạn và số tiền phạt chính xác đến từng xu. |
| **Risk Mitigation** | Kiểm soát chặt chẽ tài sản thư viện thông qua module **PenaltyTicket** và **OverdueReport**. Giảm thiểu rủi ro mất mát sách và tối ưu hóa vòng đời tài sản. |
| **Data-Driven Decision** | Cung cấp cái nhìn toàn cảnh về xu hướng đọc (**CategoryReport**) giúp Ban quản lý ra quyết định mua sắm sách dựa trên dữ liệu thực tế, không phải cảm tính. |
| **Modern Engagement** | Tích hợp tính năng **Chat & Interaction**, biến thư viện từ kho chứa sách tĩnh thành không gian tương tác sống động giữa thủ thư và độc giả. |

---

## Feature Highlights (Tính năng nổi bật)

### 1. Quản trị Tài nguyên & Kho sách (Inventory Management)
*   **Dynamic Importing:** Nhập sách thông minh (BookReceipt) với khả năng phân loại tự động theo tác giả, thể loại.
*   **Media Cloud Integration:** Lưu trữ ảnh bìa sách chất lượng cao trên nền tảng đám mây (Cloudinary), đảm bảo tốc độ tải trang cực nhanh mà không làm nặng server.

### 2. Nghiệp vụ Lưu thông (Circulation Management)
*   **Smart Lending:** Hệ thống phiếu mượn (LoanSlip) với cơ chế validate thời gian thực, ngăn chặn vi phạm quy định mượn sách ngay từ khi tạo phiếu.
*   **Penalty & Rules Engine:** Công cụ tính phạt tự động dựa trên các tham số cấu hình động (ParameterService). Quản trị viên có thể thay đổi luật phạt (ví dụ: tăng giá trị phạt quá hạn) mà **không cần sửa code**.

### 3. Báo cáo & Thống kê thông minh (Business Intelligence)
*   **Báo cáo sách trả trễ (Overdue Reporting):** Tự động tổng hợp danh sách độc giả vi phạm theo ngày/tháng.
*   **Báo cáo danh mục (Category Reporting):** Phân tích nhóm sách nào đang được quan tâm nhất.

### 4. Bảo mật & Phân quyền (Security & RBAC)
*   **Role-Based Access Control:** Phân quyền chi tiết đến từng hành động (Permissions/Roles). Đảm bảo nhân viên kho, thủ thư và quản trị viên chỉ truy cập đúng dữ liệu được phép.
*   **Secure Authentication:** Quy trình đăng nhập/đăng ký bảo mật với xác thực OTP và mã hóa Password.

---

## Target Audience (Khách hàng mục tiêu)
*   **Thư viện Đại học/Cao đẳng:** Cần quản lý lượng lớn sinh viên và tài liệu học thuật.
*   **Hệ thống Thư viện công cộng:** Cần tối ưu hóa ngân sách và phục vụ cộng đồng.
*   **Tổ chức/Doanh nghiệp:** Có tủ sách nội bộ cần quản lý việc mượn trả của nhân viên chuyên nghiệp.

---

## Technical Architecture (Kiến trúc kỹ thuật)

> *"Chúng tôi chọn công nghệ không phải vì nó 'hot', mà vì sự Ổn định (Stability) và Khả năng mở rộng (Scalability)."*

### Backend Core: .NET 6/8 (Clean Architecture)
*   **Lý do:** Đây là tiêu chuẩn vàng cho các ứng dụng doanh nghiệp nhờ tính định kiểu mạnh (Strong Typing) và hiệu năng xử lý cao.
*   **Pattern:** Sử dụng **Repository & Service Pattern** giúp tách biệt logic nghiệp vụ khỏi lớp truy cập dữ liệu, dễ dàng bảo trì và Unit Test.
*   **ORM:** Entity Framework Core giúp thao tác database an toàn, chống SQL Injection.

### Database: SQL Server
*   **Lý do:** Đảm bảo toàn vẹn dữ liệu (ACID compliance) tuyệt đối cho các giao dịch mượn trả và kế toán tiền phạt.

### Deployment: Containerization (Docker)
*   **Lý do:** Đảm bảo môi trường "Dev" giống hệt "Production". Triển khai hệ thống lên bất kỳ Server nào chỉ trong vài phút với `docker-compose`.

### Media Storage: Cloudinary
*   **Lý do:** Offload tài nguyên media ra khỏi server chính, giúp API phản hồi nhanh hơn và giảm chi phí băng thông lưu trữ.

---

## Roadmap & Vision (Tầm nhìn)

*   **Phase 1 (Current):** Hoàn thiện Core Engine lưu thông và Báo cáo cơ bản.
*   **Phase 2 (Q3/2026):** Tích hợp AI Recommendation System (Gợi ý sách dựa trên lịch sử mượn đọc của người dùng).
*   **Phase 3 (Q1/2027):** Phát triển Mobile Super App cho độc giả (Scan QR mượn sách tại kệ).

---
*Built with High-Performance Code.*
